CREATE proc [dbo].[GetSites]
as
begin	
	select	*
	from	IISSite
	where	Deleted = 0
end