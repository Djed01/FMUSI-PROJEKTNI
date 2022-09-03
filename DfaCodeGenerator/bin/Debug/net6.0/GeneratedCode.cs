using System;
using System.Collections.Generic;

public class Specification{

private HashSet<Action<string>> actions = new HashSet<Action<string>>();

public void addAction(Action<string> action){
actions.Add(action);
}

public void removeAction(Action<string> action){
actions.Remove(action);
}

public void clear(){
actions.Clear();
}

public void doStateActions(string state){
foreach(var action in actions){
action.Invoke(state);
}
}
}

public class Automat{

private string switchq0(Specification spec0, Specification spec1, Specification spec2, char symbol){
string currentState = null;
switch(symbol){
case 'a':
currentState = "q1";
spec0.doStateActions(currentState);
break;
case 'b':
currentState = "DEADSTATE";
spec1.doStateActions(currentState);
break;
default:
throw new Exception();
}
return currentState;
}

private string switchq1(Specification spec0, Specification spec1, Specification spec2, char symbol){
string currentState = null;
switch(symbol){
case 'a':
currentState = "DEADSTATE";
spec0.doStateActions(currentState);
break;
case 'b':
currentState = "q0";
spec1.doStateActions(currentState);
break;
default:
throw new Exception();
}
return currentState;
}

private string switchDEADSTATE(Specification spec0, Specification spec1, Specification spec2, char symbol){
string currentState = null;
switch(symbol){
case 'a':
currentState = "DEADSTATE";
spec0.doStateActions(currentState);
break;
case 'b':
currentState = "DEADSTATE";
spec1.doStateActions(currentState);
break;
default:
throw new Exception();
}
return currentState;
}

public void chainReaction(Specification input, Specification output,Specification spec0, Specification spec1, Specification spec2, string word){
string initState = "q0";
foreach(var symbol in word){

if(initState == "q0"){
output.doStateActions(initState);
initState = switchq0(spec0, spec1, spec2, symbol);
input.doStateActions(initState);
}

if(initState == "q1"){
output.doStateActions(initState);
initState = switchq1(spec0, spec1, spec2, symbol);
input.doStateActions(initState);
}

if(initState == "DEADSTATE"){
output.doStateActions(initState);
initState = switchDEADSTATE(spec0, spec1, spec2, symbol);
input.doStateActions(initState);
}

}
}
}
