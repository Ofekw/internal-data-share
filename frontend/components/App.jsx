import React from 'react';
import config from '../config.js';
import $ from 'jquery';
import RaisedButton from 'material-ui/RaisedButton';
import TopBar from './TopBar.jsx';
import MainBody from './MainBody.jsx';
import Paper from 'material-ui/Paper';
import FlatButton from 'material-ui/FlatButton';
import Card from './Card.jsx'

var App = React.createClass({

	onGlobalEdit: function(){
		if (this.state.editable){
			this.setState({editable: false});
		} else {
			this.setState({editable: true});
		}

	},

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
