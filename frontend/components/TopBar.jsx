import React from 'react';
import AppBar from 'material-ui/AppBar';
import IconButton from 'material-ui/IconButton';
import Edit from 'material-ui/svg-icons/editor/mode-edit';
import Save from 'material-ui/svg-icons/content/save';
import Search from 'material-ui/svg-icons/action/search';
import TextField from 'material-ui/TextField';
import $ from 'jquery';
import config from '../config.js';

// Component that renders the Top Bar and contains the Edit Mode and Search
const TopBar = React.createClass({

  // Set up initial state
  getInitialState() {
    return {
      isOpened: false
    };
  },

// Resets search bar 
/*  componentWillReceiveProps(next) {
    if(next.editButton){
      this.setState({hidden:true});
      this.refs.searchField.value = "";
    }
  },*/


  handleKeyPress: function(event){
    if (event.key === 'Enter') {
      this.handleSearchRequest(event);
    }
  },

  handleSearchRequest: function(event){
    var self = this;
    this.props.disableEditButton();
    $.get(config.apiHost+'Items/Search/Key/'+this.refs.searchField.value, function (result) {
      self.props.searchInput(result);
    });
  },

  render() {
    var searchDiv = {
      position: 'absolute',
      height: '100%',
      /* Firefox */
      width: '-moz-calc(100% - 80px)',
      /* WebKit */
      width: '-webkit-calc(100% - 80px)',
      /* Opera */
      width: '-o-calc(100% - 80px)',
      /* Standard */
      width: 'calc(100% - 80px)',
      border: 'none',
      left: 0,
      paddingTop: 3
    }

    var searchBox = {
      fontSize: 20,
      height: '60%',
      border: 'none',
      width: '100%',
      background: 'rgba(255,255,255,0.6)',
      marginLeft: 15,
      paddingLeft: 10,
    }
    //Global Icon variable 
    var icon;
    //This function changes the icon depending on the mode it is in
    if(this.props.editable){
      icon = <Save/>;
    } else {
      icon = <Edit/>;
    }
    const {isOpened} = this.state;
    return (
      <div>
        <AppBar
          showMenuIconButton={false}
          iconElementRight={
            <div>
              <div style={searchDiv}>
                <input ref='searchField' style={searchBox} onKeyPress={this.handleKeyPress}/>
              </div>
              <IconButton label='Search' onTouchTap={ this.handleSearchRequest}> <Search/></IconButton>
              <IconButton label='Edit' onTouchTap={this.props.onGlobalEdit} disabled={!this.props.editButton}>{icon}</IconButton>
            </div>
          }
          >
        </AppBar>
      </div>) 
  }
});

export default TopBar;
