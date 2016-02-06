class GameRules extends React.Component{
   render(){
       let ruleList = []; 
       this.props.rules.forEach(function(rule, index){
           ruleList.push(
               <div key={index}>{rule.CardValue} : { rule.RuleForCard } </div>
               );
       });
       
       return (
           <div> 
           <h2>RULES</h2>
           <span>{ ruleList } </span>
           </div>
       );
   } 
}

export default GameRules;