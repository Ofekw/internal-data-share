import React from 'react';
import ReactDOM from 'react-dom';
import {List, ListItem} from 'material-ui/List';
import IconMenu from 'material-ui/IconMenu';
import Popover from 'material-ui/Popover';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import IconButton from 'material-ui/IconButton/IconButton';
import MoreVertIcon from 'material-ui/svg-icons/navigation/more-vert';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import ContentCopy from 'material-ui/svg-icons/content/content-copy';
import NormalModeField from './NormalModeField.jsx';
import EditModeField from './EditModeField.jsx';

const styles = {
  paleGrey: {
    'background-color': '#ddd',
  },
};


class CardExampleExpandable extends React.Component {
  constructor(props) {
    super(props);

    this.children = [
      <NormalModeField key="1" identifier="Url" value="http://ec1-257-0-123-999.ap-northnortheast-2.compute.amazonaws.com"/>,
      <NormalModeField key="2" editable="true" identifier="Password" value="hunter2"/>,
      <NormalModeField key="3" identifier="Some other key" value="Hello World"/>,
    ];

    this.state = {
      open: false
    };
  }

  createNew = () => {
    this.children.push(<NormalModeField editable="true" key={Date.now()}/>);
    this.forceUpdate();
  }

  render() {
    return (
      <Card>
        <CardHeader
          title="ABC Bank VM_1"
          subtitle="Subtitle?"
          actAsExpander={false}
          showExpandableButton={false}
        />
        <CardActions>
          <FlatButton label="New" onTouchTap={this.createNew}/>
        </CardActions>
        <List ref="theList">
          {this.children.map(function(child, index) {
            return child;
          })}
        </List>
      </Card>
    );
  }
}
export default CardExampleExpandable;
