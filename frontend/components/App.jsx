import React from 'react';
import config from '../config.js';
import $ from 'jquery';
import RaisedButton from 'material-ui/RaisedButton';
import TopBar from './TopBar.jsx';
import MainBody from './MainBody.jsx';
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

	// Set initial state
	getInitialState: function(){
		return {
			editable: false
		}
	},

	render: function(){
		return (
			<div>
      			<TopBar onGlobalEdit={this.onGlobalEdit} editable={this.state.editable}/>
     			<MainBody editable={this.state.editable}/>
    		</div>
		)
	}
});


export default App;
