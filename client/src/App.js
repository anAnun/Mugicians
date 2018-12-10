import React, { Component } from "react";
import "./App.css";
import { Route, withRouter } from "react-router-dom";
import ViewAll from "./ViewAll";
import CreateEdit from "./CreateEdit";
import Login from "./Login";
import Store from "./Store";
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import Nav from "./Nav";

class App extends Component {
  state = {
    new: false,
    home: false
  };

  // new = () => {
  //   this.props.history.push("/create");
  // };
  // home = () => {
  //   this.history.push("/home");
  // };
  render() {
    return (
      <Provider store={Store}>
        {/* <Provider store={Store}> */}

        <BrowserRouter>
          <React.Fragment>
            <Nav />
            <Route exact path="/create" component={CreateEdit} />
            <Route exact path="/home" component={ViewAll} />
            <Route exact path="/login" component={Login} />
            <Route exact path="/:id(\d+)" component={CreateEdit} />
          </React.Fragment>
        </BrowserRouter>
      </Provider>
    );
  }
}

export default withRouter(App);
