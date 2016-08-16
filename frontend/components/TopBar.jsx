import React from 'react';
import AppBar from 'material-ui/AppBar';
import IconButton from 'material-ui/IconButton';
import FlatButton from 'material-ui/FlatButton';
import Edit from 'material-ui/svg-icons/editor/mode-edit';
import Search from 'material-ui/svg-icons/action/search';
import Collapse from 'react-collapse';
import SearchInput from 'react-search-input';

const TopBar = React.createClass({
  getInitialState() {
    return {isOpened: false};
  },

  render() {
    var divStyle = {
      width: '50%',
      margin: 'auto'
    };
    var searchBox = {
      marginTop: '10px',
      width: '100%',
      height: '35px',
      fontSize: '25px'
    }

    const {isOpened} = this.state;
    return (
      <div>
        <AppBar
          showMenuIconButton={false}
          title={<span>Title</span>}
          iconElementRight={
            <div>
              <IconButton label="Search" onTouchTap={ () => this.setState({isOpened: !isOpened}) }> <Search/></IconButton>
              <IconButton label="Edit"> <Edit/></IconButton>
            </div>
          }
          >
        </AppBar>
        <Collapse isOpened={isOpened}>
          <div style={divStyle}><SearchInput style={searchBox} onChange={this.searchUpdated} /></div>
        </Collapse>
      </div>)
  }
});

export default TopBar;