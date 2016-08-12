import React from 'react';
import {List, ListItem} from 'material-ui/List';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import ContentCopy from 'material-ui/svg-icons/content/content-copy';

const CardExampleExpandable = () => (
  <Card>
    <CardHeader
      title="ABC Bank VM_1"
      subtitle="Subtitle?"
      actAsExpander={false}
      showExpandableButton={false}
    />
    <CardActions>
      <FlatButton label="Edit" />
      <FlatButton label="Delete" />
    </CardActions>
    <List>
      <ListItem primaryText="http://ec1-257-0-123-999.ap-northnortheast-2.compute.amazonaws.com:3000" secondaryText="Url" rightIcon={<ContentCopy />} />
      <ListItem primaryText="Secret" secondaryText="Environment Password" rightIcon={<ContentCopy />} />
      <ListItem primaryText="Value 73" secondaryText="Some Other Key" rightIcon={<ContentCopy />} />
    </List>
  </Card>
);

export default CardExampleExpandable;
