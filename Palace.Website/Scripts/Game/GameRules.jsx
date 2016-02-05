class GameRules extends React.Component{
   render(){
       var ruleList = []; 
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
};