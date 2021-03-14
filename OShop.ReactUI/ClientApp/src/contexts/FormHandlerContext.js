import React, { useState, useContext, createContext, useEffect } from "react";
import { DataContext } from "./DataContext";
import axios from "axios";

export const FormHandlerContext = createContext();

const FormHandlerContextProvider = (props) => {
  const { categories, toggleReload, location, toast } = useContext(DataContext);
  const [productId, setProductId] = useState(0);
  const [categoryId, setCategoryId] = useState(0);
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [stock, setStock] = useState(0);
  const [price, setPrice] = useState(0.01);
  const [photo, setPhoto] = useState([]);
  const [categoryRefId, setCategoryRefId] = useState(0);

  useEffect(() => {
    if (categories.length !== 0) {
      setCategoryRefId(categories[0].categoryId);
    }
  }, [categories]);

  const handleInputs = (event) => {
    let name = event.target.name;
    let value = event.target.value;
    switch (name) {
      case "name":
        setName(value);
        break;
      case "description":
        setDescription(value);
        break;
      case "stock":
        setStock(value);
        break;
      case "price":
        setPrice(value);
        break;
      case "categoryRefId":
        setCategoryRefId(value);
        break;
      default:
        break;
    }
    if (name !== "categoryRefId" && name !== "photo") {
      setCustomValidity(event);
    }
  };

  const handleFile = (event) => {
    event.preventDefault();
    const file = event.target.files[0];
    if (
      !!file &&
      file.type !== "image/png" &&
      file.type !== "image/jpg" &&
      file.type !== "image/jpeg"
    ) {
      alert("You selected unaccepted format");
      event.target.files = null;
    } else {
      setPhoto(file);
    }
  };

  const showForm = () => {
    let value = document.getElementById("addUpdate").style.display;
    if (value === "none") {
      document.getElementById("addUpdate").style.display = "";
      document.getElementById("addUpdateBtn").textContent = "Hide Form";
    } else {
      document.getElementById("addUpdate").style.display = "none";
      document.getElementById("addUpdateBtn").textContent = "Show Form";
    }
  };

  const setCustomValidity = (event) => {
    event.preventDefault();
    if (event.target.value === "") {
      if (event.target.name === "name" || event.target.name === "description") {
        document.getElementById("span " + event.target.name).textContent =
          "This cannot be empty";
      } else {
        document.getElementById("span " + event.target.name).textContent =
          "Value must be a number between " +
          event.target.min +
          " and " +
          event.target.max;
      }
    } else {
      document.getElementById("span " + event.target.name).textContent = "";
    }
  };

  const resetForm = () => {
    setProductId(0);
    setCategoryId(0);
    setName("");
    setDescription("");
    setStock(0);
    setPrice(0.01);
    setPhoto([]);
    setCategoryRefId(categories[0].categoryId);
  };

  const onPost = async (event) => {
    event.preventDefault();

    const form = new FormData();
    form.append("name", name);
    form.append("photo", photo);
    if (location.pathname.includes("products")) {
      form.append("description", description);
      form.append("stock", stock);
      form.append("price", price);
      form.append("categoryRefId", categoryRefId);
      if (productId === 0) {
        await axios
          .post("/AdminPanel/createproduct", form)
          .then(() => toast.success("Added", { autoClose: 2000 }))
          .catch(() => toast.error("Error", { autoClose: 2000 }));
      } else {
        form.append("productId", productId);
        await axios
          .put("/AdminPanel/updateproduct", form)
          .then(() => toast.success("Updated", { autoClose: 2000 }))
          .catch(() => toast.error("Error", { autoClose: 2000 }));
      }
    } else {
      if (categoryId === 0) {
        await axios
          .post("/AdminPanel/createcategory", form)
          .then(() => toast.success("Added", { autoClose: 2000 }))
          .catch(() => toast.error("Error", { autoClose: 2000 }));
      } else {
        form.append("categoryId", categoryId);
        await axios
          .put("/AdminPanel/updatecategory", form)
          .then(() => toast.success("Updated", { autoClose: 2000 }))
          .catch(() => toast.error("Error", { autoClose: 2000 }));
      }
    }
    resetForm();
    toggleReload();
  };

  const onEditProduct = (product) => {
    let value = document.getElementById("addUpdate").style.display;
    if (value === "none") {
      document.getElementById("addUpdate").style.display = "";
      document.getElementById("addUpdateBtn").textContent = "Hide Form";
    }
    setProductId(product.productId);
    setName(product.name);
    setDescription(product.description);
    setStock(product.stock);
    setPrice(product.price);
    setCategoryRefId(product.categoryRefId);
  };
  const onEditCategory = (category) => {
    let value = document.getElementById("addUpdate").style.display;
    if (value === "none") {
      document.getElementById("addUpdate").style.display = "";
      document.getElementById("addUpdateBtn").textContent = "Hide Form";
    }
    setCategoryId(category.categoryId);
    setName(category.name);
  };

  return (
    <FormHandlerContext.Provider
      value={{
        productId,
        categoryId,
        name,
        description,
        stock,
        price,
        categoryRefId,
        handleFile,
        handleInputs,
        onPost,
        onEditCategory,
        onEditProduct,
        showForm,
        setCustomValidity,
        resetForm,
      }}
    >
      {props.children}
    </FormHandlerContext.Provider>
  );
};

export default FormHandlerContextProvider;
