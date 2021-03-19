import React from "react";
import Pagination from "../../Pagination";
import FormHandlerContextProvider from "../../../contexts/FormHandlerContext";
import ProductsForm from "./ProductsForm";
import ProductsTable from "./ProductsTable";

const ManageProducts = () => {
  return (
    <div>
      <h1>Product List</h1>
      <FormHandlerContextProvider>
        <ProductsTable></ProductsTable>
        <Pagination></Pagination>
        <h3 style={{ marginTop: 1 + "em" }}>Add/Update Product</h3>
        <ProductsForm></ProductsForm>
      </FormHandlerContextProvider>
    </div>
  );
};

export default ManageProducts;
