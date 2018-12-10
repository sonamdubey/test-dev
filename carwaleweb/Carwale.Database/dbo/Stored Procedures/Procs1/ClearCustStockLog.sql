IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ClearCustStockLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ClearCustStockLog]
GO

	-- =============================================
-- Author:		Garule Prabhudas
-- Create date: 26th oct,2016
-- Description:	Clear/ Delete CustStockLog after pushing it to CT
-- =============================================
CREATE PROCEDURE [dbo].[ClearCustStockLog] 
	@MaxId INT,
	@CustStockLogIdList VARCHAR(8000),
	@Delimeter VARCHAR(1)
AS
BEGIN

	SET NOCOUNT ON;
	--Delete custstocklog records picked in the last execution except network failure/cartrade db down cases so that they are executed later.
	DELETE FROM CustStockLog WHERE Id <=@MaxId AND Id NOT IN (SELECT items FROM [dbo].[SplitText](@CustStockLogIdList,@Delimeter))
END

