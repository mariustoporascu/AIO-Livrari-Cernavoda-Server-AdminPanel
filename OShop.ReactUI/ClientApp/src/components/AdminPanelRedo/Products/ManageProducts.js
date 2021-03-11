import React, { Component } from "react";
import PaginationContextProvider from "../../../contexts/PaginationContext";
import DataContextProvider from "../../../contexts/DataContext";
import Pagination from "../../PaginationRedo";
import ProductsForm from "./ProductsForm";
import ProductsTable from "./ProductsTable";

class ManageProductsRedo extends Component {
  render() {
    return (
      <DataContextProvider>
        <PaginationContextProvider>
          <h1>Product List</h1>
          <ProductsTable></ProductsTable>
          <Pagination></Pagination>
          <h2>Add/Update Product</h2>
          <ProductsForm></ProductsForm>
        </PaginationContextProvider>
      </DataContextProvider>
    );
  }
}

export default ManageProductsRedo;
