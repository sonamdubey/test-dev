IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQCateogryItems]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQCateogryItems]
GO

	-- =============================================
-- Author:		<Raghupathy>
-- Create date: <10/5/2013>
-- Description:	<This Sp is used to get LocalTax Name and Rate, IsTaxonTax field>
-- Modified by : Raghu on <10/24/2013> to get Localtax based on IsMultitax option available in PriceQuote_LocalTax Table
-- =============================================
CREATE PROCEDURE [dbo].[GetPQCateogryItems]
	-- Add the parameters for the stored procedure here
	@CityId            INT,
	@Price			   NUMERIC(18,0),
	@Id			      INT     OUTPUT,
	@Name			VARCHAR(20) OUTPUT,
	@Rate			FLOAT  OUTPUT,
	@IsTaxonTax		BIT     OUTPUT
 AS
	BEGIN
		--SELECT @Id= LT.CategoryItemid,@Name = CI.CategoryName, @Rate = Rate,@IsTaxonTax = IsTaxOnTax
		--FROM PriceQuote_LocalTax LT
		--		INNER JOIN PQ_CategoryItems CI ON CI.Id = LT.CategoryItemid
		-- WHERE CityId = @CityId
		DECLARE @IsMultiTax BIT
	    
		-- Added by Raghu for multitax Range
		SELECT @IsMultiTax = IsMultiTax
		FROM PriceQuote_LocalTax LT
		INNER JOIN PQ_CategoryItems CI ON CI.Id = LT.CategoryItemid
		WHERE CityId = @CityId

		IF (@IsMultiTax > 0 AND @Price > 0)
		BEGIN
			SELECT @Id= LT.CategoryItemid,@Name = CI.CategoryName, @Rate = LTP.Rate,@IsTaxonTax = IsTaxOnTax
			FROM PriceQuote_LocalTax LT
			INNER JOIN PQ_CategoryItems CI ON CI.Id = LT.CategoryItemid
			INNER JOIN PriceQuote_LocalTaxbyPrice LTP ON LTP.CityId = LT.CityId AND @Price >= Pricediff_Lower AND (@Price <= Pricediff_Higher OR Pricediff_Higher IS NULL) 
			WHERE LT.CityId = @CityId
        END
		ELSE
		BEGIN
			SELECT @Id= LT.CategoryItemid,@Name = CI.CategoryName, @Rate = Rate,@IsTaxonTax = IsTaxOnTax
			FROM PriceQuote_LocalTax LT
			INNER JOIN PQ_CategoryItems CI ON CI.Id = LT.CategoryItemid
			WHERE CityId = @CityId
		END
	 END



/****** Object:  StoredProcedure [dbo].[InsertShowroomPrices]    Script Date: 10/28/2013 6:12:01 PM ******/
SET ANSI_NULLS ON
