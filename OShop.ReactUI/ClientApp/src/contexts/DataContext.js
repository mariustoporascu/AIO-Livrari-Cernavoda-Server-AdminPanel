import React, { createContext, Component } from "react";
import axios from "axios";
import Loading from "../components/Loading";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

toast.configure();

export const DataContext = createContext();

class DataContextProvider extends Component {
  state = {
    productId: 0,
    categoryId: 0,
    name: "",
    description: "",
    stock: 0,
    price: 0.01,
    photo: [],
    categoryRefId: 0,
    categories: [],
    products: [],
    paginationFor: null,
    itemsPerPage: 4,
    isLoading: true,
  };

  async componentDidMount() {
    const location = window.location.pathname;
    if (location.includes("categories")) {
      this.setState({ paginationFor: "categories" });
    } else {
      this.setState({ paginationFor: "products" });
    }

    await axios
      .get("/AdminPanel/getallcategories")
      .then((response) => {
        this.setState({
          categories: response.data,
          categoryRefId: response.data[0].categoryId,
        });
      })
      .catch((error) => console.log(error));
    await axios
      .get("/AdminPanel/getallproducts")
      .then((response) => {
        this.setState({ products: response.data, isLoading: false });
      })
      .catch((error) => console.log(error));
  }

  handleInputs = (event) => {
    let name = event.target.name;
    let value = event.target.value;
    this.setState({ [name]: value });
    if (name !== "categoryRefId" && name !== "photo") {
      this.setCustomValidity(event);
    }
  };
  handleFile = (event) => {
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
      this.setState({ photo: file });
    }
  };

  editProduct = (product) => {
    this.setState({
      productId: product.productId,
      name: product.name,
      description: product.description,
      stock: product.stock,
      price: product.price,
      categoryRefId: product.categoryRefId,
    });
  };
  editCategory = (category) => {
    this.setState({
      categoryId: category.categoryId,
      name: category.name,
      description: category.description,
    });
  };

  setCustomValidity = (event) => {
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
  toggleLoading = () => {
    this.setState({ isLoading: !this.state.isLoading });
  };

  postContent = async () => {
    let type;
    const form = new FormData();
    form.append("name", this.state.name);
    form.append("photo", this.state.photo);
    if (this.state.paginationFor === "products") {
      form.append("description", this.state.description);
      form.append("stock", this.state.stock);
      form.append("price", this.state.price);
      form.append("categoryRefId", this.state.categoryRefId);
      if (this.state.productId === 0) {
        await axios
          .post("/AdminPanel/createproduct", form)
          .then(() => toast.success("Added", { autoClose: 2000 }))
          .catch(() => toast.error("Error", { autoClose: 2000 }));
        type = "post";
      } else {
        form.append("productId", this.state.productId);
        await axios
          .put("/AdminPanel/updateproduct", form)
          .then(() => toast.success("Updated", { autoClose: 2000 }))
          .catch(() => toast.error("Error", { autoClose: 2000 }));
        type = "put";
      }
      await axios
        .get("/AdminPanel/getallproducts")
        .then((response) => {
          this.setState({
            products: response.data,
          });
        })
        .catch((error) => console.log(error));
    } else {
      if (this.state.categoryId === 0) {
        await axios
          .post("/AdminPanel/createcategory", form)
          .then(() => toast.success("Added", { autoClose: 2000 }))
          .catch(() => toast.error("Error", { autoClose: 2000 }));
        type = "post";
      } else {
        form.append("categoryId", this.state.categoryId);
        await axios
          .put("/AdminPanel/updatecategory", form)
          .then(() => toast.success("Updated", { autoClose: 2000 }))
          .catch(() => toast.error("Error", { autoClose: 2000 }));
        type = "put";
      }
      await axios
        .get("/AdminPanel/getallcategories")
        .then((response) => {
          this.setState({
            categories: response.data,
          });
        })
        .catch((error) => console.log(error));
    }

    this.resetForm();

    return type;
  };

  removeItem = async (id) => {
    if (this.state.paginationFor === "products") {
      var prodIndex = this.state.products.findIndex(
        (obj) => obj.productId === id
      );
      this.state.products.splice(prodIndex, 1);
      await axios
        .delete("/AdminPanel/deleteproduct/" + id)
        .then(() => toast.success("Removed", { autoClose: 2000 }))
        .catch(() => toast.error("Error", { autoClose: 2000 }));
    } else {
      var categIndex = this.state.categories.findIndex(
        (obj) => obj.categoryId === id
      );
      this.state.categories.splice(categIndex, 1);
      await axios
        .delete("/AdminPanel/deletecategory/" + id)
        .then(() => toast.success("Removed", { autoClose: 2000 }))
        .catch(() => toast.error("Error", { autoClose: 2000 }));
    }
  };

  resetForm = () => {
    if (this.state.paginationFor === "products") {
      this.setState({
        productId: 0,
        name: "",
        description: "",
        stock: 0,
        price: 0.01,
        photo: [],
        categoryRefId: this.state.categories[0].categoryId,
      });
    } else {
      this.setState({
        categoryId: 0,
        name: "",
      });
    }
  };

  render() {
    let page = this.state.isLoading ? (
      <Loading />
    ) : (
      <DataContext.Provider
        value={{
          ...this.state,
          handleInputs: this.handleInputs,
          handleFile: this.handleFile,
          setCustomValidity: this.setCustomValidity,
          editProduct: this.editProduct,
          editCategory: this.editCategory,
          removeItem: this.removeItem,
          postContent: this.postContent,
        }}
      >
        {this.props.children}
      </DataContext.Provider>
    );
    return <div>{page}</div>;
  }
}

export default DataContextProvider;
