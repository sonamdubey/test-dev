IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_GetDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_GetDealers]
GO

	




-------------------------------------------------------------------------------------------


-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <16/10/2014>
-- Description:	<Get DealerNames to auto suggest and if new name save to temp table>
-- =============================================
CREATE PROCEDURE [dbo].[DD_GetDealers]
@SearchText      VARCHAR(50),
@Id				 INT,
@DealerName		 VARCHAR(100),
@CreatedBy		 INT = null,
@DealerId		 INT OUTPUT
AS
BEGIN
	SET @DealerId = -1
	IF @Id = -1
	BEGIN
		SELECT Name FROM DD_DealerNames WHERE Name LIKE @SearchText + '%' 
	END
	ELSE IF @Id = 1
	BEGIN
		SELECT @DealerId = ID from DD_DealerNames WHERE Name = @DealerName 
		IF @@ROWCOUNT < 1
		BEGIN
			IF NOT EXISTS (SELECT Id FROM DD_TempDealerNames WHERE Name = @DealerName AND IsActive = 1)
			BEGIN
				INSERT INTO DD_TempDealerNames(Name,IsActive,CreatedOn,CreatedBy) VALUES(@DealerName,1,GETDATE(),@CreatedBy) 
			END
		END
	END
END

