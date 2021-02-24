import React, { Component } from "react";
import Form from "./Form";
import CategoriesTable from "./CategoriesTable";
import axios from "axios";
import Pagination from "../../Pagination";

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
      loadingItems: true
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
      .then(response => {
        this.setState({
          itemsArray: response.data,
          loadingForm: false
        });
      })
      .catch(error => console.log(error));
    await this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5)
    });
    var pagination = Pagination.pagination(1, this.state.itemsArray, 5);
    await this.setState({
      currPage: 1,
      pageItems: pagination.filter,
      loadingItems: false
    });
  }

  handleChange = (nam, val) => {
    this.setState({ [nam]: val });
  };

  async handleLoading() {
    await this.setState({ loadingForm: true, loadingItems: true });
  }

  async changePage(pageNmbr) {
    var pagination = Pagination.pagination(pageNmbr, this.state.itemsArray, 5);
    await this.setState({ currPage: pageNmbr, pageItems: pagination.filter });
  }

  handleCategory = category => {
    this.setState({
      categoryId: category.categoryId,
      name: category.name
    });
  };

  async removeCategory(name) {
    await this.setState({ loadingItems: true });
    await axios
      .delete("/AdminPanel/deletecategory/" + name)
      .then(response => console.log(response))
      .catch(error => console.log(error));
    await axios
      .get("/AdminPanel/getallcategories")
      .then(response => {
        this.setState({
          itemsArray: response.data
        });
      })
      .catch(error => console.log(error));
    await this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5)
    });
    if (this.state.totalPages.length < this.state.currPage) {
      let pagination = Pagination.pagination(
        this.state.currPage - 1,
        this.state.itemsArray,
        5
      );
      await this.setState({
        currPage: pagination.pageNmbr,
        pageItems: pagination.filter,
        loadingItems: false
      });
    } else {
      let pagination = Pagination.pagination(
        this.state.currPage,
        this.state.itemsArray,
        5
      );
      await this.setState({
        pageItems: pagination.filter,
        loadingItems: false
      });
    }
  }

  async updateAfterPost(pageNmbr) {
    await this.setState({
      categoryId: 0,
      name: "",
      photo: []
    });
    await axios
      .get("/AdminPanel/getallcategories")
      .then(response => {
        this.setState({
          itemsArray: response.data,
          loadingForm: false
        });
      })
      .catch(error => console.log(error));
    await this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5)
    });
    if (this.state.totalPages.length > pageNmbr) {
      let pagination = Pagination.pagination(
        this.state.totalPages.length,
        this.state.itemsArray,
        5
      );
      await this.setState({
        currPage: pagination.pageNmbr,
        pageItems: pagination.filter,
        loadingItems: false
      });
    } else {
      let pagination = Pagination.pagination(
        pageNmbr,
        this.state.itemsArray,
        5
      );
      await this.setState({
        currPage: pagination.pageNmbr,
        pageItems: pagination.filter,
        loadingItems: false
      });
    }
  }

  async updateAfterPut(pageNmbr) {
    await this.setState({
      categoryId: 0,
      name: "",
      photo: []
    });
    await axios
      .get("/AdminPanel/getallcategories")
      .then(response => {
        this.setState({
          itemsArray: response.data,
          loadingForm: false
        });
      })
      .catch(error => console.log(error));
    await this.setState({
      totalPages: Pagination.pageHelper(this.state.itemsArray, 5)
    });
    let pagination = Pagination.pagination(pageNmbr, this.state.itemsArray, 5);
    await this.setState({
      currPage: pagination.pageNmbr,
      pageItems: pagination.filter,
      loadingItems: false
    });
  }
  async find(event) {
    var value = event.target.value;
    await this.setState({ loadingItems: true });
    if (!!value) {
      await this.setState({
        findArray: this.state.itemsArray.filter(product =>
          product.name.toLowerCase().includes(value.toLowerCase())
        )
      });
      await this.setState({
        totalPages: Pagination.pageHelper(this.state.findArray, 5)
      });
      let pagination = Pagination.pagination(1, this.state.findArray, 5);
      await this.setState({
        currPage: 1,
        pageItems: pagination.filter,
        loadingItems: false
      });
    } else {
      await this.setState({
        totalPages: Pagination.pageHelper(this.state.itemsArray, 5)
      });
      let pagination = Pagination.pagination(1, this.state.itemsArray, 5);
      await this.setState({
        currPage: 1,
        pageItems: pagination.filter,
        loadingItems: false
      });
    }
  }

  render() {
    let categoriesContents = this.state.loadingItems ? (
      <p>
        <em>Loading categories...</em>
      </p>
    ) : (
      <CategoriesTable
        counterStates={this.state}
        passEditCategory={this.handleCategory}
        passRemoveCategory={this.removeCategory}
      />
    );
    let formContents = this.state.loadingForm ? (
      <p>
        <em>refreshing form...</em>
      </p>
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
