import React, { createContext, useState, useEffect } from "react";
import axios from "axios";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useLocation } from "react-router-dom";

//import { LoginActions } from "../components/api-authorization/ApiAuthorizationConstants";

export const DataContext = createContext();

const DataContextProvider = (props) => {
  const [categories, setCategories] = useState([]);
  const [products, setProducts] = useState([]);

  const [isLoading, setIsLoading] = useState(true);

  const [findString, setFindString] = useState("");

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
        setIsLoading(!isLoading);
      }
    };
    populateData();
  }, [isLoading]);

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
        setProducts(newArray);
        break;
      default:
        break;
    }
  };

  const toggleReload = () => {
    setIsLoading(!isLoading);
  };

  const findItems = (event) => {
    let name = event.target.value;
    setFindString(name);
  };

  return (
    <DataContext.Provider
      value={{
        categories,
        products,
        removeItem,
        toggleReload,
        findString,
        findItems,
        location,
        toast,
      }}
    >
      {props.children}
    </DataContext.Provider>
  );
};

export default DataContextProvider;
