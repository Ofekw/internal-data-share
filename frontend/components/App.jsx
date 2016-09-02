import React from 'react';
import config from '../config.js';
import $ from 'jquery';
import RaisedButton from 'material-ui/RaisedButton';
import TopBar from './TopBar.jsx';
import MainBody from './MainBody.jsx';
import Paper from 'material-ui/Paper';
import FlatButton from 'material-ui/FlatButton';
import Card from './Card.jsx'

class App extends React.Component {
  constructor(props) {
    super(props);

    this.leafChildren = [];
  }

  handleTouchTap() {
    alert('hello');
  }

  componentDidMount() {
    this.enableLoadingMode;
    this.serverRequest = $.get(config.apiHost + 'GetApiItemsById.json', function (result) {
      // console.log(result);
      // result = JSON.parse(result);
      this.setState({
        leafChildren: result.LeafChildren
      });
    }.bind(this));
  }

  render() {
    return (<div>
      <TopBar/>
      <Card leafChildren={this.state ? this.state.leafChildren : []} />
    </div>);
  }
}

export default App;
