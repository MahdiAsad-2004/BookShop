If
OBJECT_ID('dbo.CalculateDiscounterPrice' , 'FN') Is Null
Begin
	Exec('
		Create FUNCTION CalculateDiscounterPrice(@price int , @discountPrice int NULL, @discountPercent int NULL) RETURNS real 
		AS
			BEGIN
				Declare @DiscountedPrice real = Null;
				Declare @discountPriceFloat real = Cast(@discountPrice As real);
				Declare @discountPercentFloat real = Cast(@discountPercent As real);
				Declare @priceFloat real = Cast(@price As real);


				Set @DiscountedPrice =
					Case 
						When @discountPercent Is Not Null Then CAST(@priceFloat - (@priceFloat  * @discountPercentFloat / 100) As float(24))
						When @discountPrice Is Not Null Then CAST(@priceFloat - @discountPriceFloat As real)
					End

				Return @DiscountedPrice
			END
	')
End

