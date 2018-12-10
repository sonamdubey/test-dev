IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerBankView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerBankView]
GO

	
/****** Object:  StoredProcedure [dbo].[TC_DealerBankSave]    Script Date: 10/05/2011 15:32:56 ******/
-- =============================================
-- Author:		SURENDRA CHOUKSEY
-- Create date: 5th October 2011
-- Description:	This procedure will be used to View All Dealers Bank for Finance
-- =============================================
CREATE PROCEDURE [dbo].[TC_DealerBankView]
(
@DealerId NUMERIC
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT TC_DealerBank_Id ,BankName FROM TC_DealerBank WHERE DealerId=@DealerId AND IsActive=1	
    
END


