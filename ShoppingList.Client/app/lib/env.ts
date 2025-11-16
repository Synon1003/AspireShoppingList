import { z } from 'zod'

import tryParseEnv from './try-parse-env'

export const EnvSchema = z.object({
  NODE_ENV: z.string().default('development'),
  API_URL: z.string().default('https://localhost:7065'),
  PAGE_SIZE: z.coerce.number().int().min(1).max(20).default(5),
})

export type Env = z.infer<typeof EnvSchema>

tryParseEnv(EnvSchema)

export default EnvSchema.parse((globalThis as any).process?.env ?? {})