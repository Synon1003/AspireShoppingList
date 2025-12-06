import { describe, it, expect, beforeEach, afterEach } from 'vitest'
import { shallowMount } from '@vue/test-utils'
import Toast from '~/components/Toast.vue'

describe('Toast.vue', () =>
{
    let wrapper

    beforeEach(() => wrapper = null)

    afterEach(() =>
    {
        if (wrapper) wrapper.unmount()
    })

    const testCases = [
        { type: 'success', class: '.alert-success' },
        { type: 'error', class: '.alert-error' },
        { type: 'info', class: '.alert-info' }
    ]

    testCases.forEach(({ type, class: alertClass }) =>
    {
        it(`renders ${type} alert when type is "${type}"`, () =>
        {
            wrapper = shallowMount(Toast, {
                props: {
                    type: type,
                    message: 'Message'
                }
            })

            expect(wrapper.find(alertClass).exists()).toBe(true)
            expect(wrapper.text()).toContain('Message')
        })
    })
})