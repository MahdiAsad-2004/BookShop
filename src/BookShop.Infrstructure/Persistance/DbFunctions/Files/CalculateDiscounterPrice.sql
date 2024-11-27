If
OBJECT_ID('dbo.CalculateDiscounterPrice' , 'FN') Is Null
Begin
	Exec('
		Create FUNCTION CalculateDiscounterPrice(@price int , @discountPrice int NULL, @discountPercent int NULL) RETURNS real 
		AS
			BEGIN
				Declare @DiscountedPrice real = Null;
				Declare @discountPriceFloat float(24) = Cast(@discountPrice As Float(24));
				Declare @discountPercentFloat float(24) = Cast(@discountPercent As Float(24));
				Declare @priceFloat float(24) = Cast(@price As Float(24));


				Set @DiscountedPrice =
					Case 
						When @discountPercent Is Not Null Then CAST(@priceFloat - (@priceFloat  * @discountPercentFloat / 100) As float(24))
						When @discountPrice Is Not Null Then CAST(@priceFloat - @discountPriceFloat As Float(24))
					End

				Return @DiscountedPrice
			END
	')
End

