import React from 'react';
import RaisedButton from 'material-ui/RaisedButton';
import TopBar from './TopBar.jsx';
import MainBody from './MainBody.jsx';
import Paper from 'material-ui/Paper';
import FlatButton from 'material-ui/FlatButton';

class App extends React.Component {
  handleTouchTap() {
    alert('hello');
  }

  render() {
    return <div>
      <TopBar/>
      <MainBody/>
    </div>;
  }
}

export default App;
