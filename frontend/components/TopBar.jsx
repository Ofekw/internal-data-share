import React from 'react';
import AppBar from 'material-ui/AppBar';
import IconButton from 'material-ui/IconButton';
import TextField from 'material-ui/TextField';
import DropDownMenu from 'material-ui/DropDownMenu';
import MenuItem from 'material-ui/MenuItem';
import $ from 'jquery';
import config from '../config.js';

//Icons
import Edit from 'material-ui/svg-icons/editor/mode-edit';
import Save from 'material-ui/svg-icons/content/save';
import Search from 'material-ui/svg-icons/action/search';

// Component that renders the Top Bar and contains the Edit Mode and Search
const TopBar = React.createClass({

  // Set up initial state
  getInitialState() {
    return {
      isOpened: false,
      value: 1
    };
  },

// Resets search bar 
 componentWillReceiveProps(next) {
    if(next.editButton){
      this.refs.searchField.value = "";
    }
  },


  handleKeyPress: function(event){
    if (event.key === 'Enter') {
      this.handleSearchRequest(event);
    }
  },

  handleSearchRequest: function(event){
    var self = this;
    this.props.disableEditButton();
    var text = this.refs.searchField.value;
    if(!text.trim()){
      return;
    }
    var searchType = '';
    if (this.state.value === 1){
      searchType = 'Items/Search/Key/'
    }
    else if (this.state.value === 2){
      searchType = 'Items/Search/Value/'
    }
    else if (this.state.value === 3){
      searchType = 'Items/Search/Note/'
    }
    else{
      searchType = 'Items/Search/Label/'
    }
    $.get(config.apiHost + searchType + this.refs.searchField.value, function (result) {
      self.props.searchInput(result);
    });
  },

  handleSearchTypeChange(event, index, value){
    this.setState({value});
  },

  render() {
    var searchDiv = {
      position: 'absolute',
      height: '100%',
      /* Firefox */
      width: '-moz-calc(100% - 210px)',
      /* WebKit */
      width: '-webkit-calc(100% - 210px)',
      /* Opera */
      width: '-o-calc(100% - 210px)',
      /* Standard */
      width: 'calc(100% - 210px)',
      border: 'none',
      left: 0,
      paddingTop: 3
    };

    var searchBox = {
      fontSize: 20,
      height: '60%',
      border: 'none',
      width: '100%',
      background: 'rgba(255,255,255,0.6)',
      marginLeft: 15,
      paddingLeft: 10,
    };

    var searchButtonStyle = {
      position: 'absolute',
      right: 185
    }

    var dropDownDiv = {
      position: 'absolute',
      right: 50,
      top: 5
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
          iconElementLeft={
            <div>
              <div style={searchDiv}>
                <input ref='searchField' style={searchBox} onKeyPress={this.handleKeyPress} disabled={this.props.editable}/>
              </div>
              <IconButton label='Search' onTouchTap={ this.handleSearchRequest} style={searchButtonStyle}><Search/></IconButton>
            </div>
          }
          iconElementRight={
            <div>
            <div style={dropDownDiv}>
              <DropDownMenu
                value={this.state.value}
                autoWidth={false}
                underlineStyle={{visibility: 'hidden'}}
                onChange={this.handleSearchTypeChange}
              >
                <MenuItem value={1} primaryText="Node/Key" />
                <MenuItem value={2} primaryText="Value" />
                <MenuItem value={3} primaryText="Notes" />
                <MenuItem value={4} primaryText="Labels" />
              </DropDownMenu>
              </div>
              <IconButton label='Edit' onTouchTap={this.props.onGlobalEdit} disabled={!this.props.editButton}>{icon}</IconButton>
            </div>
          }
          >
        </AppBar>
      </div>) 
  }
});

export default TopBar;
