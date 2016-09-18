import React from 'react';
import AppBar from 'material-ui/AppBar';
import IconButton from 'material-ui/IconButton';
import Edit from 'material-ui/svg-icons/editor/mode-edit';
import Save from 'material-ui/svg-icons/content/save';
import Search from 'material-ui/svg-icons/action/search';
import TextField from 'material-ui/TextField';
import $ from 'jquery';

// Component that renders the Top Bar and contains the Edit Mode and Search
const TopBar = React.createClass({

  // Set up initial state
  getInitialState() {
    return {
      isOpened: false,
      hidden: true,
    };
  },

  // Handles the new addition of an list item
	handleTouchTap: function(){
    if (this.state.hidden === true){
      this.setState({hidden:false});
    }
    else {
      this.setState({hidden:true});
    }
		
	},

  render() {
    var searchDiv = {
      WebkitTransition: 'width 0.4s ease-in-out',
      transition: 'width 0.4s ease-in-out',
      MozTransition: 'width 0.4s ease-in-out',
      OTransition: 'width 0.4s ease-in-out',
      height: '100%',
      width: 0,
      position: 'absolute',
      overflow: 'hidden',
      whiteSpace: 'nowrap',
      border: 'none',
      right: 55,
      paddingTop: 3
    }

    var searchBox = {
      fontSize: 20,
      height: '60%',
      border: 'none',
      width: '100%',
      background: 'rgba(255,255,255,0.6)',
      marginLeft: 5
    }
    //Global Icon variable 
    var icon;
    //This function changes the icon depending on the mode it is in
    if(this.props.editable){
      icon = <Save/>;
    } else {
      icon = <Edit/>;
    }
    if (this.state.hidden === true){
      searchDiv.width = 0;
    }
    else {
      searchDiv.width = '82%';
    }

    const {isOpened} = this.state;
    return (
      <div>
        <AppBar
          showMenuIconButton={false}
          iconElementRight={
            <div>
              <div style={searchDiv}>
                <input id='searchField' style={searchBox}/>
              </div>
              <IconButton label='Search' onTouchTap={ this.handleTouchTap }> <Search/></IconButton>
              <IconButton label='Edit' ref='editButton' onTouchTap={this.props.onGlobalEdit}> {icon}</IconButton>
            </div>
          }
          >
        </AppBar>
      </div>)
  }
});

export default TopBar;
