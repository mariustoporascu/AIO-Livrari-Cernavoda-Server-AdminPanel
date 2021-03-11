import React, { Component } from "react";
import PaginationContextProvider from "../../../contexts/PaginationContext";
import DataContextProvider from "../../../contexts/DataContext";
import Pagination from "../../PaginationRedo";
import CategoriesForm from "./CategoriesForm";
import CategoriesTable from "./CategoriesTable";

class ManageCategoriesRedo extends Component {
  render() {
    return (
      <DataContextProvider>
        <PaginationContextProvider>
          <h1>Categories List</h1>
          <CategoriesTable></CategoriesTable>
          <Pagination></Pagination>
          <h2>Add/Update Category</h2>
          <CategoriesForm></CategoriesForm>
        </PaginationContextProvider>
      </DataContextProvider>
    );
  }
}

export default ManageCategoriesRedo;
