import React, { Component } from "react";
import { Route } from "react-router";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { ManageProducts } from "./components/AdminPanel/Products/ManageProducts";
import { ManageCategories } from "./components/AdminPanel/Categories/ManageCategories";
import { CartInfo } from "./components/ShoppingCart/CartInfo";

import AuthorizeRoute from "./components/api-authorization/AuthorizeRoute";
import ApiAuthorizationRoutes from "./components/api-authorization/ApiAuthorizationRoutes";
import { ApplicationPaths } from "./components/api-authorization/ApiAuthorizationConstants";

import "./custom.css";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path="/" component={Home} />
        <AuthorizeRoute
          path="/adminpanel/manageproducts"
          component={ManageProducts}
        />
        <AuthorizeRoute
          path="/adminpanel/managecategories"
          component={ManageCategories}
        />
        <Route
          path={ApplicationPaths.ApiAuthorizationPrefix}
          component={ApiAuthorizationRoutes}
        />
        <Route path="/shoppingcart" component={CartInfo} />
      </Layout>
    );
  }
}
