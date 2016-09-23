import React from 'react';
import config from '../config.js';
import $ from 'jquery';
import RaisedButton from 'material-ui/RaisedButton';
import TopBar from './TopBar.jsx';
import ParentContainer from './List/ParentContainer.jsx';
import Paper from 'material-ui/Paper';
import FlatButton from 'material-ui/FlatButton';
import Card from './Card.jsx'

// Component that renders the top bar and main body
var App = React.createClass({

	// Tells the body that the edit button has been pressed
	onGlobalEdit: function(){
		if (this.state.editable){
			this.setState({editable: false});
		} else {
			this.setState({editable: true});
		}
	},

	searchInput: function(result){
		this.setState({searchResult: result});
	},

	enableEditButton(){
		this.setState({editButton: true, searchResult: ""});
	},

	disableEditButton(){
		this.setState({editButton:false})
	},

	// Set initial state
	getInitialState: function(){
		return {
			editable: false,
			editButton: true,
			searchResult: ""
		}
	},

	render: function(){
		return (
			<div>
      			<TopBar onGlobalEdit={this.onGlobalEdit} editable={this.state.editable} searchInput={this.searchInput} disableEditButton={this.disableEditButton} editButton={this.state.editButton}/>
     			<ParentContainer editable={this.state.editable} searchResult={this.state.searchResult} enableEditButton={this.enableEditButton} />
    		</div>
		)
	}
});


export default App;
