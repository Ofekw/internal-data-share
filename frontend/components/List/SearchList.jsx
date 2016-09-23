import React from 'react';
import {List, ListItem} from 'material-ui/List';

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
                    if (result.IsLeaf){
                        return <ListItem
                            primaryText={result.Key}
                            key={result.Id}
                            secondaryText={result.Value}
                            onClick={this.onClick.bind(this,result)}
                        />
                    }
                    return <ListItem 
                        primaryText={result.Key}
                        key={result.Id}
                        onClick={this.onClick.bind(this,result)}
                    />
                })
            }
            </List>
        );
    }
});

export default SearchList;