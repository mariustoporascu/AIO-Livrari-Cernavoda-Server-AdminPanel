import React, { createContext, useState, useEffect } from "react";
import axios from "axios";
import Loading from "../components/Loading";
import { toast } from "react-toastify";
import { useLocation } from "react-router-dom";
import "react-toastify/dist/ReactToastify.css";
//import { LoginActions } from "../components/api-authorization/ApiAuthorizationConstants";

export const DataContext = createContext();

const DataContextProvider = (props) => {
  const [categories, setCategories] = useState([]);
  const [products, setProducts] = useState([]);

  const [isLoading, setIsLoading] = useState(true);

  const [totalPages, setTotalPages] = useState([]);
  const [currPage, setCurrPage] = useState(1);

  toast.configure();
  const location = useLocation();

  useEffect(() => {
    const populateData = async () => {
      await axios
        .get("/AdminPanel/getallcategories")
        .then((response) => {
          setCategories(response.data);
        })
        .catch((error) => console.log(error));
      await axios
        .get("/AdminPanel/getallproducts")
        .then((response) => {
          setProducts(response.data);
        })
        .catch((error) => console.log(error));
      if (isLoading) {
        setIsLoading(false);
      }
    };
    populateData();
  }, [isLoading]);

  useEffect(() => {
    let paginationItems;
    let itemsPerPage;

    if (categories.length !== 0 && products.length !== 0) {
      switch (location.pathname) {
        case "/adminpanel/managecategories":
          paginationItems = categories;
          itemsPerPage = 4;
          break;
        case "/adminpanel/manageproducts":
          paginationItems = products;
          itemsPerPage = 4;
          break;
        default:
          paginationItems = products;
          itemsPerPage = 12;
          break;
      }
      let totalItems = paginationItems.length;
      let totPages = Math.ceil(totalItems / itemsPerPage);

      let pages = [];
      for (let i = 0; i < totPages; i++) {
        pages.push(i + 1);
      }
      setTotalPages(pages);
    }
  }, [products, categories, location]);

  useEffect(() => {
    setCurrPage(1);
  }, [location]);

  const changePage = (value) => {
    setCurrPage(value);
  };

  const removeItem = async (id) => {
    let newArray;
    switch (location.pathname) {
      case "/adminpanel/managecategories":
        await axios
          .delete("/AdminPanel/deletecategory/" + id)
          .then(() => toast.success("Removed", { autoClose: 2000 }))
          .catch(() => toast.error("Error", { autoClose: 2000 }));
        newArray = categories.filter((categ) => categ.categoryId !== id);
        setCategories(newArray);

        break;
      case "/adminpanel/manageproducts":
        await axios
          .delete("/AdminPanel/deleteproduct/" + id)
          .then(() => toast.success("Removed", { autoClose: 2000 }))
          .catch(() => toast.error("Error", { autoClose: 2000 }));
        newArray = products.filter((prod) => prod.productId !== id);
        break;
      default:
        break;
    }
  };

  const toggleReload = () => {
    setIsLoading(true);
  };

  return (
    <DataContext.Provider
      value={{
        categories,
        products,
        removeItem,
        toggleReload,
        totalPages,
        currPage,
        changePage,
        location,
        toast,
      }}
    >
      {props.children}
    </DataContext.Provider>
  );
};

export default DataContextProvider;
