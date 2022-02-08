public class WhileStatement : AbstractStatement {

  public override string Keyword { get; init; } = "while"; 
  //public Expression conditional { get; set; };
  
    public WhileStatement() {
    var that = this;
    this.Keyword = "while";
    }

    public override string ToXML() {
        //return "<while>" + conditional + "</while>";
        return "";
    }
}