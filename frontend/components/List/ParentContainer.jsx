import React from 'react';
import ListNode from './ListNode.jsx';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';


var ParentContainer = React.createClass({

	

	getInitialState: function() {
		return {
			parent: null,
			breadcrumbs : [{
				id: "",
				name: "Home"
			}]
		}
	},

	handleClick: function(item,cb) {
		this.setState({parent: item}, function(){
			console.log(this.state.parent);
			cb();
		});
		this.state.breadcrumbs.push({
			id : item,
			name:item
		})
	},


	render: function(){
		return (
			<div>
				{
					this.state.breadcrumbs.map( crumb => {
						return <span><FlatButton  label={crumb.name} onClick={this.breadcrumbClick.bind(this,crumb)}/> ></span>
					})
				 }
			 	<ListNode parent={this.state.parent} handleClick={this.handleClick}/>

		 	</div>
		)
	},

	breadcrumbClick: function(crumb){ 
		
		for (var i = this.state.breadcrumbs.length-1; i > 0; i--){
			if (this.state.breadcrumbs[i].id === crumb.id){
				this.state.breadcrumbs.pop();
			}
		}
		
		var parent = this.state.breadcrumbs[this.state.breadcrumbs.length];
		this.setState({parent: parent})
	}
});



export default ParentContainer;