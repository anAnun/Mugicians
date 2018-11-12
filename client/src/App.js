import React, { Component } from "react";
import "./App.css";
import { Route, withRouter } from "react-router-dom";
import ViewAll from "./ViewAll";
import CreateEdit from "./CreateEdit";

class App extends Component {
  state = {
    new: false,
    home: false
  };

  new = () => {
    this.props.history.push("/create");
  };
  home = () => {
    this.history.push("/home");
  };
  render() {
    return (
      <React.Fragment>
        <div className="image">
          <div className="App-header piano">
            <button
              className={"nav"}
              onClick={() => this.props.history.push("/home")}
            >
              Home
            </button>
            <button
              className={"nav"}
              onClick={() => this.props.history.push("/create")}
            >
              New Song
            </button>
          </div>
        </div>
        <Route exact path="/home" component={ViewAll} />
        <Route exact path="/create" component={CreateEdit} />
        <Route exact path="/:id(\d+)" component={CreateEdit} />
      </React.Fragment>
    );
  }
}

export default withRouter(App);
