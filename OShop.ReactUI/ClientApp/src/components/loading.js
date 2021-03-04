/*eslint unicode-bom: ["error", "always"]*/
import React, { Component } from "react";

export default class Loading extends Component {
  render() {
    return (
      <div>
        <div className="text-center">
          <div className="spinner-border" role="status">
            <span className="sr-only">Loading...</span>
          </div>
        </div>
      </div>
    );
  }
}
