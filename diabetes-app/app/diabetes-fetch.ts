export default async <T, B>(
	url: string,
	method = "get",
	body: B | undefined = undefined,
	headers = {}
): Promise<T | { error: string }> => {
	const controller = new AbortController();
	try {
		const res = await fetch(`${url}`, {
			method: method.toUpperCase(),
			signal: controller.signal,
			body: typeof body === "object" ? JSON.stringify(body) : undefined,
			mode: "cors",
			headers: {
				"Content-type": "application/json",
				Accept: "*/*",
				"Access-Control-Allow-Origin": "*",
				"Referrer-policy": "unsafe-url",
				"Cache-Control": "no-cache",
				Pragma: "no-cache",
				"Sec-Fetch-Dest": "document",
				"Sec-Fetch-Mode": "navigate",
				"Sec-Fetch-Site": "none",
				"Allow-origin": "*",
				"Access-Control-Request-Headers":
					"access-control-allow-origin,allow-origin,api-secret,cache-control,content-type,pragma,referrer-policy",
				...headers,
			},
		});
		if (!res.ok) {
			console.log("IT FAILED");
			const error = await res.json();
			return { error: error.code };
		}
		console.log("IT PASSED");
		return await res.json();
	} catch (err) {
		return { error: err };
	} finally {
		controller.abort();
	}
};
