import React from "react";

import Pagination from "../../Pagination";
import FormHandlerContextProvider from "../../../contexts/FormHandlerContext";
import CategoriesForm from "./CategoriesForm";
import CategoriesTable from "./CategoriesTable";

const ManageCategories = () => {
  return (
    <div>
      <h1>Categories List</h1>
      <FormHandlerContextProvider>
        <CategoriesTable></CategoriesTable>
        <Pagination></Pagination>
        <h3 style={{ marginTop: 1 + "em" }}>Add/Update Category</h3>
        <CategoriesForm></CategoriesForm>
      </FormHandlerContextProvider>
    </div>
  );
};

export default ManageCategories;
