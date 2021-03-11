/*eslint unicode-bom: ["error", "always"]*/
import React, { Component } from "react";
import Form from "./Form";
import CategoriesTable from "./CategoriesTable";
import axios from "axios";
import Pagination from "../../Pagination";
import Loading from "../../Loading";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

toast.configure();

export class ManageCategories extends Component {
  static displayName = ManageCategories.name;
  constructor(props) {
    super(props);
    this.state = {
      categoryId: 0,
      name: "",
      photo: [],
      itemsArray: [],
      findArray: [],
      pageItems: [],
      totalPages: [],
      currPage: null,
      loadingForm: true,
      loadingItems: true,
    };
    this.handleLoading = this.handleLoading.bind(this);
    this.removeCategory = this.removeCategory.bind(this);
    this.updateAfterPost = this.updateAfterPost.bind(this);
    this.updateAfterPut = this.updateAfterPut.bind(this);
    this.changePage = this.changePage.bind(this);
    this.find = this.find.bind(this);
  }

  async componentDidMount() {
    await axios
      .get("/AdminPanel/getallcategories")
      .then((response) => {
        this.setState({
          itemsArray: response.data,
          loadingForm: false,
        });
      })
      .catch((error) => console.log(error));
    this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5),
    });
    var pagination = Pagination.pagination(1, this.state.itemsArray, 5);
    this.setState({
      currPage: 1,
      pageItems: pagination.filter,
      loadingItems: false,
    });
  }

  handleChange = (nam, val) => {
    this.setState({ [nam]: val });
  };

  async handleLoading() {
    this.setState({ loadingForm: true, loadingItems: true });
  }

  async changePage(pageNmbr) {
    var pagination = Pagination.pagination(pageNmbr, this.state.itemsArray, 5);
    this.setState({ currPage: pageNmbr, pageItems: pagination.filter });
  }

  handleCategory = (category) => {
    this.setState({
      categoryId: category.categoryId,
      name: category.name,
    });
  };

  async removeCategory(name) {
    this.setState({ loadingItems: true });
    await axios
      .delete("/AdminPanel/deletecategory/" + name)
      .then(() => toast.success("Removed", { autoClose: 2000 }))
      .catch(() => toast.error("Error", { autoClose: 2000 }));

    var categIndex = this.state.itemsArray.findIndex(
      (obj) => obj.name === name
    );

    this.state.itemsArray.splice(categIndex, 1);
    this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5),
    });
    if (this.state.totalPages.length < this.state.currPage) {
      let pagination = Pagination.pagination(
        this.state.currPage - 1,
        this.state.itemsArray,
        5
      );
      this.setState({
        currPage: pagination.pageNmbr,
        pageItems: pagination.filter,
        loadingItems: false,
      });
    } else {
      let pagination = Pagination.pagination(
        this.state.currPage,
        this.state.itemsArray,
        5
      );
      this.setState({
        pageItems: pagination.filter,
        loadingItems: false,
      });
    }
  }
  resetForm = () => {
    this.setState({ categoryId: 0, name: "", photo: [] });
  };
  async updateAfterPost(pageNmbr) {
    await axios
      .get("/AdminPanel/getallcategories")
      .then((response) => {
        this.setState({
          itemsArray: response.data,
        });
      })
      .catch((error) => console.log(error));
    this.resetForm();
    this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5),
      loadingForm: false,
    });
    if (this.state.totalPages.length > pageNmbr) {
      let pagination = Pagination.pagination(
        this.state.totalPages.length,
        this.state.itemsArray,
        5
      );
      this.setState({
        currPage: pagination.pageNmbr,
        pageItems: pagination.filter,
        loadingItems: false,
      });
    } else {
      let pagination = Pagination.pagination(
        pageNmbr,
        this.state.itemsArray,
        5
      );
      this.setState({
        currPage: pagination.pageNmbr,
        pageItems: pagination.filter,
        loadingItems: false,
      });
    }
  }

  async updateAfterPut(pageNmbr) {
    await axios
      .get("/AdminPanel/getallcategories")
      .then((response) => {
        this.setState({
          itemsArray: response.data,
        });
      })
      .catch((error) => console.log(error));
    let pagination = Pagination.pagination(pageNmbr, this.state.itemsArray, 5);
    this.resetForm();
    this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5),
      currPage: pagination.pageNmbr,
      pageItems: pagination.filter,
      loadingItems: false,
      loadingForm: false,
    });
  }
  async find(event) {
    var value = event.target.value;
    this.setState({ loadingItems: true });
    if (!!value) {
      this.setState({
        findArray: this.state.itemsArray.filter((product) =>
          product.name.toLowerCase().includes(value.toLowerCase())
        ),
      });
      this.setState({
        totalPages: Pagination.pageHelper(this.state.findArray, 5),
      });
      let pagination = Pagination.pagination(1, this.state.findArray, 5);
      this.setState({
        currPage: 1,
        pageItems: pagination.filter,
        loadingItems: false,
      });
    } else {
      this.setState({
        totalPages: Pagination.pageHelper(this.state.itemsArray, 5),
      });
      let pagination = Pagination.pagination(1, this.state.itemsArray, 5);
      this.setState({
        currPage: 1,
        pageItems: pagination.filter,
        loadingItems: false,
      });
    }
  }

  render() {
    let categoriesContents = this.state.loadingItems ? (
      <Loading />
    ) : (
      <CategoriesTable
        counterStates={this.state}
        passEditCategory={this.handleCategory}
        passRemoveCategory={this.removeCategory}
      />
    );
    let formContents = this.state.loadingForm ? (
      ""
    ) : (
      <Form
        passHandler={this.handleChange}
        passLoading={this.handleLoading}
        counterStates={this.state}
        passUpdPost={this.updateAfterPost}
        passUpdPut={this.updateAfterPut}
      />
    );
    let pagination = (
      <Pagination counterStates={this.state} passChangePage={this.changePage} />
    );

    return (
      <div>
        <h1 id="tabelLabel">Categories List</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {formContents}
        <input
          type="text"
          name="find"
          placeholder="Search category"
          onChange={this.find}
        />
        {categoriesContents}
        {pagination}
      </div>
    );
  }
}
