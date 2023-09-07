using ProjectLab.Models.Entities;
using ProjectLab.Models.View;
using ProjectLab.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using Lucene.Net.Search;

namespace ProjectLab.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        private readonly IShoppingCartService _shoppingCartService;

        private readonly IUserService _userService;


        public ProductsController(IProductService productService, IShoppingCartService shoppingCartService, IUserService userService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _userService = userService;
        }

        [Authorize]
        public IActionResult AddToCart(int productId)
        {
            if (_userService.IsUserLoggedIn(User))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _shoppingCartService.AddProductInCart(userId, productId);
                Product product = _productService.GetProductById(productId);
                return RedirectToAction("Menu");
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        [HttpGet]
        public IActionResult List(int productType, string? currentFilter)
        {
            var model = new ProductListModelView();

            var products = _productService.GetAllByType(productType);
            if (_userService.IsUserLoggedIn(User))
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<ProductModelView> productsList = currentFilter == null || currentFilter.Equals("") ? products.ToList() :
                    products.Where(p => p.Name.ToLower().Contains(currentFilter.ToLower())).ToList();
                products = productsList.AsEnumerable();
            }

            model.Products = products;
            model.PageProductTypeId = productType;
            model.PageTitle = _productService
                .GetProductTypes()
                .ToList()
                .Find(p => p.Id == productType)
                .Name + "  Menu";

            return View("~/Views/Products/Index.cshtml", model);
        }

        [HttpGet]
        public IActionResult Menu()
        {
            return View("~/Views/Products/Menu.cshtml");
        }

        [HttpGet]
        public IActionResult SearchProducts(int productType, string term)
        {
            var products = _productService.GetAllByType(productType);
            List<string> productsList = term == null || term.Equals("") ? products.Select(p => p.Name).ToList() :
                    products.Where(p => p.Name.ToLower().Contains(term.ToLower())).Select(p => p.Name).ToList();
            return Json(productsList);
        }

        [HttpGet]
        public IActionResult SearchProductsByLucene(int productType, string? searchTerm)
        {
            var model = new ProductListModelView();
            var products = _productService.GetProductsByType(productType);
            var productsView = new List<ProductModelView>();
            if (searchTerm != null)
            {

                string indexDir = @"C:\LuceneIndex";
                if (!System.IO.Directory.Exists(indexDir))
                {
                    System.IO.Directory.CreateDirectory(indexDir);
                }
            
                Utils.LuceneUtil.IndexPDFs(products.ToList());

                var dir = FSDirectory.Open(new DirectoryInfo(indexDir));
                var searcher = new IndexSearcher(dir);
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

                // Parse the search term into a Lucene Query object
                var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new string[] { "name", "text" }, analyzer);
                var query = parser.Parse(searchTerm);

                // Execute the query and get the top 10 results sorted by score
                var collector = TopScoreDocCollector.Create(10, true);
                searcher.Search(query, collector);
                var hits = collector.TopDocs().ScoreDocs;
                foreach (var hit in hits)
                {
                    var doc = searcher.Doc(hit.Doc);
                    productsView.Add(_productService.GetProductModelView(int.Parse(doc.Get("id"))));
                }
            }
            else
            {
                productsView = _productService.GetAllByType(productType).ToList();
            }


            model.Products = productsView;
            model.PageProductTypeId = productType;
            model.PageTitle = _productService.GetProductTypes().ToList().Find(p => p.Id == productType).Name + "  Menu";

            return View("~/Views/Products/Index.cshtml", model);
        }

        // GET: Products/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? productId)
        {
            if (productId == null)
            {
                return NotFound();
            }
            else
            {
                var product = _productService.GetProductById((int)productId);

                if (product == null)
                {
                    return NotFound();
                }

                return View(_productService.GetProductModelView((int)productId));
            }

        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([
            Bind("Name,Description,Price,ProductImage")] ProductModelView model,
            List<IFormFile> productImage)
        {
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
            };

            try
            {
                bool ok = false;
                if (productImage.Count > 0)
                {
                    foreach (var image in productImage)
                    {
                        string fileName = image.FileName;
                        string fileExtension = Path.GetExtension(fileName);
                        if (fileExtension.Equals(".png") || fileExtension.Equals(".jpg") ||
                            fileExtension.Equals(".jpeg"))
                        {
                            ok = true;
                            using (var stream = new MemoryStream())
                            {
                                await image.CopyToAsync(stream);
                                product.Photo = stream.ToArray();
                            }
                        }
                    }

                    if (ok)
                    {
                        _productService.AddProduct(product);
                        return RedirectToAction("Menu");
                    }



                }

            }
            catch (Exception ex)
            {

            }

            return View(model);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetProductModelView((int)id);

            if (product == null)
            {
                return NotFound();
            }
           
            return View();
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Photo")] ProductModelView model,
            List<IFormFile> productImage)
        {
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price
            };

            try
            {
                bool ok = false;
                if (productImage.Count > 0)
                {
                    foreach (var image in productImage)
                    {
                        string fileName = image.FileName;
                        string fileExtension = Path.GetExtension(fileName);
                        if (fileExtension.Equals(".png") || fileExtension.Equals(".jpg") ||
                            fileExtension.Equals(".jpeg"))
                        {
                            ok = true;
                            using (var stream = new MemoryStream())
                            {
                                await image.CopyToAsync(stream);
                                product.Photo = stream.ToArray();
                            }
                        }
                    }

                    if (ok)
                    {
                        _productService.UpdateProduct(product);
                        return RedirectToAction("Details", new RouteValueDictionary(
                             new { controller = "Products", action = "Details", productId = product.Id }));
                    }



                }

            }
            catch (Exception ex)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetProductById((int)id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product product = _productService.GetProductById(id);
            product.isDeleted = true;
            _productService.UpdateProduct(product);
            return RedirectToAction("Menu");
        }

        private bool ProductExists(int id)
        {
            return _productService.GetProductById(id) != null;
        }
    }
}
