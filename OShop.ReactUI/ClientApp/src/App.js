import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { ManageProducts } from './components/ManageProducts';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
            <Route path='/manageproducts' component={ManageProducts} />
        <Route path='/fetch-data' component={FetchData} />
      </Layout>
    );
  }
}
