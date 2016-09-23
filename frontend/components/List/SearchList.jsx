import React from 'react';
import {List, ListItem} from 'material-ui/List';
import Divider from 'material-ui/Divider';

var SearchList = React.createClass({
    onClick: function (item){
        var result = item;
        if (this.props.editable){
            return
        }
        this.props.searchResultClick(result,true);
	},
    
    render(){
        return (
            <List>
            {
                this.props.searchResult.map(result => {
                    var path = "Home ";
                    result.Path.forEach(function(element, index, array) {
                        path = path.concat(" > ");
                        path = path.concat(element.Value);
                    }, this);
                    if (result.IsLeaf){
                        return <div key={result.Id}> <Divider/> <ListItem primaryText={path} key={result.Id} secondaryText={result.Value} onClick={this.onClick.bind(this,result)}/> </div>
                    }
                    return <div key={result.Id}> <Divider/> <ListItem primaryText={path} key={result.Id} onClick={this.onClick.bind(this,result)}/> </div>
                })
            }
            </List>
        );
    }
});

export default SearchList;