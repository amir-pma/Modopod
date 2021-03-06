//import page components
import Catalogue from "../pages/Catalogue.vue";
import Product from "../pages/Product.vue";
import Cart from "../pages/Cart.vue";
import Checkout from "../pages/Checkout.vue";
import Account from "../pages/Account.vue";

//import admin pages
import AdminIndex from "../pages/admin/Index.vue";
import AdminOrders from "../pages/admin/Orders.vue";
import AdminProducts from "../pages/admin/Products.vue";
import AdminCreateProduct from "../pages/admin/CreateProduct.vue";
import MakeProduct from "../pages/MakeProduct.vue";
import Login from "../components/app/Login.vue"

const routes = [
  { path: "/login", component: Login },
    { path: "/makeproduct", component: MakeProduct, meta: { requiresAuth: true } },
  { path: "/products/:slug", component: Product },
  { path: "/cart", component: Cart, meta: { role: "Customer" } },
  {
    path: "/checkout",
    component: Checkout,
    meta: { requiresAuth: true, role: "Customer" }
  },
  {
    path: "/account",
    component: Account,
    meta: { requiresAuth: true, role: "Customer" }
  },
  {
    path: "/admin",
    component: AdminIndex,
    meta: { requiresAuth: true, role: "Admin" },
    redirect: "/admin/orders",
    children: [
      {
        path: "orders",
        component: AdminOrders
      },
      {
        path: "products",
        component: AdminProducts
      },
      {
        path: "products/create",
        component: AdminCreateProduct
      }
    ]
  },
  { path: "*", redirect: "/login" }
];

export default routes;
