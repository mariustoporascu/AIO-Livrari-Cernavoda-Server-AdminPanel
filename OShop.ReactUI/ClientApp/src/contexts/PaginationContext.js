import React, { createContext, Component } from "react";
import { DataContext } from "./DataContext";

export const PaginationContext = createContext();

class PaginationContextProvider extends Component {
  state = { pageItems: [], totalPages: [], currPage: null };

  static contextType = DataContext;

  componentDidMount() {
    this.pageHelper();
    this.pageConstr(1, null);
  }

  pageConstr = (pageNmbr) => {
    const { products, categories, paginationFor, itemsPerPage } = this.context;
    let totalItems;
    let paginationItems;
    if (paginationFor === "categories") {
      totalItems = categories.length;
      paginationItems = categories;
    } else {
      totalItems = products.length;
      paginationItems = products;
    }
    let currentPageIndex = pageNmbr * itemsPerPage;
    let i;
    let filter = [];
    if (currentPageIndex > totalItems)
      for (i = currentPageIndex - itemsPerPage; i < totalItems; i++) {
        filter.push(paginationItems[i]);
      }
    else {
      for (i = currentPageIndex - itemsPerPage; i < currentPageIndex; i++) {
        filter.push(paginationItems[i]);
      }
    }
    this.setState({ pageItems: filter, currPage: pageNmbr });
  };

  pageHelper = () => {
    const { products, categories, paginationFor, itemsPerPage } = this.context;
    let totalItems;
    if (paginationFor === "categories") {
      totalItems = categories.length;
    } else {
      totalItems = products.length;
    }

    let totPages = Math.ceil(totalItems / itemsPerPage);
    let i;
    let pages = [];
    for (i = 1; i < totPages + 1; i++) {
      pages.push(i);
    }

    this.setState({ totalPages: pages });
  };

  render() {
    const { postContent, removeItem } = this.context;
    return (
      <PaginationContext.Provider
        value={{
          ...this.state,
          pageHelper: this.pageHelper,
          pageConstr: this.pageConstr,
          postContent: postContent,
          removeItem: removeItem,
        }}
      >
        {this.props.children}
      </PaginationContext.Provider>
    );
  }
}

export default PaginationContextProvider;
