import React, { useState, useEffect, createContext, useContext } from "react";
import { DataContext } from "./DataContext";

export const PaginationContext = createContext();

const PaginationContextProvider = (props) => {
  const { categories, products, location, findString } = useContext(
    DataContext
  );

  const [contextItems, setContextItems] = useState([]);
  const [paginationItems, setPaginationItems] = useState([]);
  const [pageItems, setPageItems] = useState([]);
  const [itemsPerPage, setItemsPerPage] = useState(0);
  const [totalPages, setTotalPages] = useState([]);
  const [currPage, setCurrPage] = useState(1);

  useEffect(() => {
    switch (location.pathname) {
      case "/adminpanel/managecategories":
        setItemsPerPage(4);
        setContextItems(categories);
        break;
      case "/adminpanel/manageproducts":
        setItemsPerPage(4);
        setContextItems(products);
        break;
      case "/":
        setItemsPerPage(12);
        setContextItems(products);
        break;
      default:
        break;
    }
  }, [products, categories, location.pathname]);

  useEffect(() => {
    if (contextItems.length !== 0) {
      if (!!findString) {
        setPaginationItems(
          contextItems.filter((obj) =>
            obj.name.toLowerCase().includes(findString.toLowerCase())
          )
        );

        changePage(1);
      } else {
        setPaginationItems(contextItems);
      }
    }
  }, [findString, contextItems]);

  useEffect(() => {
    if (itemsPerPage !== 0 && paginationItems.length !== 0) {
      let totalItems = paginationItems.length;
      let totPages = Math.ceil(totalItems / itemsPerPage);

      let pages = [];
      for (let i = 0; i < totPages; i++) {
        pages.push(i + 1);
      }
      setTotalPages(pages);
    }
  }, [paginationItems, itemsPerPage]);

  useEffect(() => {
    if (itemsPerPage !== 0 && paginationItems.length !== 0) {
      let finish = currPage * itemsPerPage;
      let start = currPage - 1;
      setPageItems(paginationItems.slice(start * itemsPerPage, finish));
    }
  }, [currPage, paginationItems, itemsPerPage]);

  useEffect(() => {
    setCurrPage(1);
  }, [location.pathname]);

  const changePage = (value) => {
    setCurrPage(value);
  };

  return (
    <PaginationContext.Provider
      value={{ pageItems, totalPages, currPage, changePage }}
    >
      {props.children}
    </PaginationContext.Provider>
  );
};

export default PaginationContextProvider;
