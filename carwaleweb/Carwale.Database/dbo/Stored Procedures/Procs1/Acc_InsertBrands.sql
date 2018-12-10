IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_InsertBrands]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_InsertBrands]
GO

	
-- PROCEDUTRE TO 

CREATE PROCEDURE [dbo].[Acc_InsertBrands]
	@BrandName			VARCHAR(100),
	@LogoUrl		 	VARCHAR(100),
	@WebUrl			VARCHAR(100),
	@TollFreeNo			VARCHAR(10),
	@BrandId			INTEGER OUTPUT
 AS
	
BEGIN
	SELECT  BrandName FROM Acc_Brands WHERE BrandName = @BrandName

	IF @@RowCount = 0
	
		BEGIN
			INSERT INTO Acc_Brands ( BrandName, LogoUrl , WebUrl, TollFreeNo )
			VALUES ( @BrandName, @LogoUrl , @WebUrl, @TollFreeNo)	
			
			SET @BrandId = SCOPE_IDENTITY()
			
			IF @BrandId > 0 AND @LogoUrl <> ''
				BEGIN
					UPDATE Acc_Brands SET LogoUrl = Convert( VarChar, @BrandId ) +  '.jpg' WHERE Id = @BrandId
				END
		END
	ELSE
		SET @BrandId = -1
	
END
