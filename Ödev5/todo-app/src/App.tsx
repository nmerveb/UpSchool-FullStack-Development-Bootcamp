import React, { useEffect, useState } from "react";
import "semantic-ui-css/semantic.min.css";
import "./App.css";
import {
  Button,
  Card,
  Container,
  Header,
  Image,
  Input,
} from "semantic-ui-react";
import ToDoCard from "./components/ToDoCard";

type CardData = {
  id: number;
  header: string;
  isCompleted: boolean;
  deadline: string;
};

const App = () => {
  const [toDoList, setToDoList] = useState<CardData[]>([]);
  const [newToDo, setNewToDo] = useState("");
  const [deadline, setDeadline] = useState("");
  const [isAddDisabled, setIsAddDisabled] = useState(true);

  useEffect(() => {
    const storedToDoList = localStorage.getItem("toDoList");
    if (storedToDoList) {
      try {
        setToDoList(JSON.parse(storedToDoList));
      } catch (error) {
        console.error("Error parsing stored ToDo list:", error);
      }
    }
  }, []);

  useEffect(() => {
    try {
      localStorage.setItem("toDoList", JSON.stringify(toDoList));
    } catch (error) {
      console.error("Error storing ToDo list in localStorage:", error);
    }
  }, [toDoList]);

  useEffect(() => {
    setIsAddDisabled(!(newToDo && deadline));
  }, [newToDo, deadline]);

  const addToDo = () => {
    const newCardData: CardData = {
      id: Math.random(),
      header: newToDo,
      isCompleted: false,
      deadline: deadline,
    };

    setToDoList((prevToDoList) => [...prevToDoList, newCardData]);
    setNewToDo("");
    setDeadline("");
  };

  const handleDoubleClick = (id: number) => {
    setToDoList((prevToDoList) =>
      prevToDoList.map((toDo) =>
        toDo.id === id ? { ...toDo, isCompleted: !toDo.isCompleted } : toDo
      )
    );
  };

  const sortList = () => {
    setToDoList((prevToDoList) =>
      [...prevToDoList].sort(
        (a, b) =>
          new Date(a.deadline).getTime() - new Date(b.deadline).getTime()
      )
    );
  };

  const handleRemove = (id: number) => {
    setToDoList((prevToDoList) =>
      prevToDoList.filter((toDo) => toDo.id !== id)
    );
  };

  return (
    <Container>
      <Header
        as="h1"
        style={{
          marginTop: "10%",
          marginBottom: "5%",
          textAlign: "center",
          color: "#3054a5",
          fontSize: "2.5rem",
        }}
      >
        <Image
          src="/ToDoLogo.png"
          alt="Logo"
          style={{
            fontSize: "2rem",
          }}
        />
        Small steps lead to big wins!
      </Header>

      <div
        style={{
          margin: "-.875em -0.5em",
          display: "flex",
          flexWrap: "wrap",
          justifyContent: "space-evenly",
          marginBottom: "1rem",
        }}
      >
        <Input
          placeholder="ToDo"
          value={newToDo}
          onChange={(e) => setNewToDo(e.target.value)}
        />
        <Input
          type="date"
          placeholder="Deadline"
          value={deadline}
          onChange={(e) => setDeadline(e.target.value)}
        />
        <Button primary size="large" onClick={addToDo} disabled={isAddDisabled}>
          Add
        </Button>

        <Button size="large" color="teal" onClick={sortList}>
          Sort
        </Button>
      </div>

      <Card.Group>
        {toDoList.map((toDo) => (
          <ToDoCard
            key={toDo.id}
            id={toDo.id}
            header={toDo.header}
            isCompleted={toDo.isCompleted}
            deadline={toDo.deadline}
            onDoubleClick={handleDoubleClick}
            onRemove={handleRemove}
          />
        ))}
      </Card.Group>
    </Container>
  );
};

export default App;
