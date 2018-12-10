IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetStateByCityId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetStateByCityId]
GO

	-- =============================================
-- Created date : 1st OCt, 2015
-- Description   : To Get State From City id
-- Used By	:	Rohan Sapkal
-- EXEC [dbo].[GetStateByCityId] 1
-- =============================================
CREATE PROCEDURE [dbo].[GetStateByCityId] 
	@CityID INT
AS
BEGIN
select S.ID,S.NAME from 
Cities C WITH (NOLOCK) 
INNER JOIN States S WITH (NOLOCK) on C.StateId=S.ID
where C.ID = @CityID
END


