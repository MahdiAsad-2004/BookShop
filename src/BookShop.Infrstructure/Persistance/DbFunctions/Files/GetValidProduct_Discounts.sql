
If
OBJECT_ID('dbo.GetValidProduct_Discounts' , 'TF') Is Null
Begin
	Exec('
		Create Function dbo.GetValidProduct_Discounts() Returns Table
		Return (
			Select [Product_Discounts].[DiscountId], [Product_Discounts].[ProductId], Discounts.DiscountPrice , Discounts.DiscountPercent , Discounts.MaximumUseCount ,
			Discounts.[UsedCount] , Discounts.[StartDate] , Discounts.[EndDate] ,Discounts.[Priority] , Discounts.[Name]
			From Discounts 
			Left Join [Product_Discounts] On Discounts.Id = [Product_Discounts].DiscountId
			Where 
				(Discounts.MaximumUseCount Is Null Or Discounts.MaximumUseCount > Discounts.UsedCount) And
				(Discounts.StartDate Is Null Or Discounts.StartDate < GETDATE()) And
				(Discounts.EndDate Is Null Or Discounts.EndDate > GETDATE()) And
				(Discounts.DiscountPrice Is Not Null Or Discounts.DiscountPercent Is Not Null)

		);
	')
End

--[Product_Discounts].[DiscountId], [Product_Discounts].[ProductId], Discounts.DiscountPrice , Discounts.DiscountPercent , Discounts.MaximumUseCount ,
--			Discounts.[UsedCount] , Discounts.[StartDate] , Discounts.[EndDate] ,Discounts.[Priority] , Discounts.[Name]



