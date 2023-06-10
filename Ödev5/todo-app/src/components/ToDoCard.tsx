import React from "react";
import { Card, Icon } from "semantic-ui-react";

function ToDoCard() {
  return (
    <Card fluid>
      <Card.Content>
        <Card.Header>Başlık 2</Card.Header>
        <Card.Description> Deadline : 01.01.1999</Card.Description>
      </Card.Content>
      <Icon
        name="close"
        style={{
          position: "absolute",
          top: "10px",
          right: "10px",
          cursor: "pointer",
          color: "red",
        }}
      />
    </Card>
  );
}

export default ToDoCard;
