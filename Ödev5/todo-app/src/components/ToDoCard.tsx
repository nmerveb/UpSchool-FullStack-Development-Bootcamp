import React from "react";
import { Card, Icon } from "semantic-ui-react";

type ToDoCardProps = {
  id: number;
  header: string;
  isCompleted: boolean;
  deadline: string;
  onDoubleClick: (id: number) => void;
  onRemove: (id: number) => void;
};

const ToDoCard: React.FC<ToDoCardProps> = ({
  id,
  header,
  isCompleted,
  deadline,
  onDoubleClick,
  onRemove,
}) => {
  const handleDoubleClick = () => {
    onDoubleClick(id);
  };

  const handleRemove = () => {
    onRemove(id);
  };

  return (
    <Card
      fluid
      onDoubleClick={handleDoubleClick}
      style={{
        textDecoration: isCompleted ? "line-through" : "none",
      }}
    >
      <Card.Content>
        <Card.Header>{header}</Card.Header>
        <Card.Description>Deadline: {deadline}</Card.Description>
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
        onClick={handleRemove}
      />
    </Card>
  );
};

export default ToDoCard;
