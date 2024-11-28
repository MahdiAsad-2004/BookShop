
If
OBJECT_ID('dbo.GetProductsWithReviewsAverageScore' , 'TF') Is Null
Begin
	Exec('
		Create Function dbo.GetProductsWithReviewsAverageScore() Returns Table
		Return (
			 Select R.ProductId ,
   				Cast(Avg(CAST([r].Score As real)) As real) As [AverageScore] ,
   				CAST(Avg(CAST((Case When [r].IsAccepted = 1 Then [r].Score End) As real)) As real) As [AcceptedAverageScore] 
			From 
				Reviews As R
			Group By 
				R.ProductId 
		);
	')
End







